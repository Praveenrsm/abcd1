// ProductDetails.js
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';

const ProductDetails = () => {
  const { productId } = useParams();
  const navigate = useNavigate();

  const [product, setProduct] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Review states
  const [rating, setRating] = useState(0);
  const [comments, setComments] = useState('');
  const [reviewLoading, setReviewLoading] = useState(false);
  const [userReview, setUserReview] = useState(null);

  
  // 🔐 Get logged-in userId from JWT
  const getUserIdFromToken = () => {
    const token = localStorage.getItem("token");
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split(".")[1]));
      return payload[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
      ];
    } catch {
      return null;
    }
  };

  const handleAddToCart = async () => {
  try {
    const token = localStorage.getItem("token");
    const userId = getUserIdFromToken();

    const cartPayload = {
      productId: product.productId,
      quantity: 1
    };

    await axios.post(
      "https://localhost:7292/api/Cart/AddCart",
      cartPayload,
      {
        params: {
          userId: userId // null for guest
        },
        headers: token
          ? { Authorization: `Bearer ${token}` }
          : {}
      }
    );

    alert("Product added to cart");
  } catch (err) {
    console.error(err);
    alert("Failed to add to cart");
  }
};

  // 🔄 Fetch product + detect user review
  const fetchProductDetails = async () => {
    try {
      const response = await axios.get(
        `https://localhost:7292/api/Products/GetProductWithReviews/${productId}`
      );

      const productData = response.data;
      setProduct(productData);

      const loggedInUserId = getUserIdFromToken();

      if (loggedInUserId && productData.reviews) {
        const existingReview = productData.reviews.find(
          r => r.user && r.user.userId === loggedInUserId
        );

        if (existingReview) {
          setUserReview(existingReview);
          setRating(existingReview.rating);
          setComments(existingReview.comments);
        } else {
          setUserReview(null);
          setRating(0);
          setComments('');
        }
      }
    } catch (err) {
      console.error(err);
      setError("Failed to load product details");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchProductDetails();
  }, [productId]);

  // ⭐ Add / Update review
  const handleAddOrUpdateReview = async () => {
    const token = localStorage.getItem("token");
    const userId = getUserIdFromToken();

    if (!token || !userId) {
      alert("Please login to add a review");
      return;
    }

    if (rating < 1 || rating > 5) {
      alert("Rating must be between 1 and 5");
      return;
    }

    try {
      setReviewLoading(true);

      await axios.post(
        "https://localhost:7292/api/RatingReview/AddOrUpdateReview",
        null,
        {
          params: {
            productId: product.productId,
            comments,
            rating
          },
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      );

      alert(userReview ? "Review updated successfully" : "Review added successfully");

      fetchProductDetails();
    } catch (err) {
      console.error(err);
      alert("Failed to submit review");
    } finally {
      setReviewLoading(false);
    }
  };

  if (loading) return <p>Loading product details...</p>;
  if (error) return <p>{error}</p>;
  if (!product) return <p>Product not found</p>;

  return (
    <div className="product-details-page">
      <button onClick={() => navigate(-1)}>← Back</button>

      <h2>{product.productName}</h2>

      {/* Image */}
      {product.productImages?.length > 0 ? (
        <img
          src={`data:image/png;base64,${product.productImages[0].imageData}`}
          alt={product.productName}
          style={{ maxWidth: "300px" }}
        />
      ) : (
        <div>No image available</div>
      )}

      <p>{product.description}</p>
      <p><strong>Price:</strong> ₹{product.productPrice}</p>
      <button onClick={handleAddToCart}>
  Add to Cart
</button>

      <p><strong>Available Quantity:</strong> {product.availableQuantity}</p>

      <hr />

      {/* Reviews */}
      <h3>Reviews</h3>
      {product.reviews?.length > 0 ? (
        <ul>
          {product.reviews.map(review => (
            <li key={review.id}>
              <strong>{review.user?.userName}</strong> ⭐ {review.rating}
              <p>{review.comments}</p>
            </li>
          ))}
        </ul>
      ) : (
        <p>No reviews available</p>
      )}

      <hr />

      {/* Add / Update Review */}
      {localStorage.getItem("token") ? (
        <>
          <h3>{userReview ? "Update Your Review" : "Add Your Review"}</h3>

          <label>Rating (1–5)</label>
          <input
            type="number"
            min="1"
            max="5"
            value={rating}
            onChange={e => setRating(Number(e.target.value))}
          />

          <label>Comments</label>
          <textarea
            value={comments}
            onChange={e => setComments(e.target.value)}
          />

          <button onClick={handleAddOrUpdateReview} disabled={reviewLoading}>
            {reviewLoading
              ? "Submitting..."
              : userReview
                ? "Update Review"
                : "Add Review"}
          </button>
        </>
      ) : (
        <p>Please login to add a review</p>
      )}
    </div>
  );
};

export default ProductDetails;
