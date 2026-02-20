// Products.js
import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const Products = () => {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

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

  const handleAddToCart = async (productId, e) => {
    e.stopPropagation(); // 🔴 IMPORTANT: prevent card click navigation

    try {
      const token = localStorage.getItem("token");
      const userId = getUserIdFromToken();

      const cartPayload = {
        productId,
        quantity: 1
      };

      await fetch("https://localhost:7292/api/Cart/AddCart", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          ...(token && { Authorization: `Bearer ${token}` })
        },
        body: JSON.stringify(cartPayload)
      });

      alert("Product added to cart");
    } catch (err) {
      console.error(err);
      alert("Failed to add to cart");
    }
  };

  useEffect(() => {
    fetch('https://localhost:7292/api/Products/GetAllProducts')
      .then((response) => {
        if (!response.ok) throw new Error('Failed to fetch products');
        return response.json();
      })
      .then((data) => {
        setProducts(data);
        setLoading(false);
      })
      .catch((error) => {
        setError(error.message);
        setLoading(false);
      });
  }, []);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;

  return (
    <div className="products-page">
      <h1>Products</h1>

      <div className="products-list">
        {products.map(product => (
          <div
            key={product.productId}
            className="product-card"
            onClick={() =>
              navigate(`/product/ProductDetails/${product.productId}`)
            }
          >
            <h2>{product.productName}</h2>
            <p>{product.description}</p>
            <p>₹{product.productPrice.toFixed(2)}</p>

            {product.productImages?.length > 0 ? (
              <img
                src={`data:image/png;base64,${product.productImages[0].imageData}`}
                alt={product.productName}
              />
            ) : (
              <div>No image</div>
            )}

            {/* 🛒 ADD TO CART */}
            <button
              onClick={(e) => handleAddToCart(product.productId, e)}
            >
              Add to Cart
            </button>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Products;
