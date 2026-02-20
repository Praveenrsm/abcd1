import React, { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const Cart = () => {
  const [cartItems, setCartItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  const token = localStorage.getItem("token");

  const fetchCart = async () => {
    try {
      const res = await axios.get(
        "https://localhost:7292/api/Cart/GetCart",
        {
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      );
      setCartItems(res.data);
    } catch (err) {
      console.error("Fetch cart error", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCart();
  }, []);

  // 🔁 Update quantity (+ / -)
  const updateQuantity = async (productId, newQuantity) => {
    try {
      await axios.put(
        "https://localhost:7292/api/Cart/UpdateQuantity",
        null,
        {
          params: { productId, quantity: newQuantity },
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      );
      fetchCart(); // refresh cart
    } catch (err) {
      console.error("Update quantity error", err);
    }
  };

  if (loading) return <p>Loading cart...</p>;

  if (cartItems.length === 0)
    return <p>Your cart is empty</p>;
const cartTotal = cartItems.reduce(
  (sum, item) => sum + item.totalPrice,
  0
);

  return (
    <div className="cart-page">
      <h2>Your Cart</h2>

      {cartItems.map(item => (
        <div className="cart-item" key={item.id}>
          {/* 🔗 Clickable product */}
          <img
            src={
              item.product?.productImages?.length > 0
                ? `data:image/png;base64,${item.product.productImages[0].imageData}`
                : ""
            }
            alt={item.product?.productName}
            onClick={() =>
              navigate(`/product/ProductDetails/${item.product.productId}`)
            }
            style={{ cursor: "pointer" }}
          />

          <div className="cart-info">
            <h4
              style={{ cursor: "pointer", color: "#007185" }}
              onClick={() =>
                navigate(`/product/ProductDetails/${item.product.productId}`)
              }
            >
              {item.product?.productName}
            </h4>

            <p>Price: ₹{item.product?.productPrice}</p>

            {/* ➕ ➖ Quantity */}
            <div className="qty-controls">
              <button onClick={() => updateQuantity(item.productId, item.quantity - 1)}>
                −
              </button>

              <span>{item.quantity}</span>

              <button onClick={() => updateQuantity(item.productId, item.quantity + 1)}>
                +
              </button>
            </div>

            <p><strong>Total:</strong> ₹{item.totalPrice}</p>
          </div>
        </div>
        

      ))}
      <div className="cart-summary">
  <h3>Grand Total: ₹{cartTotal}</h3>

  <button
    className="buy-now-btn"
    onClick={() => navigate("/selectaddress")}
  >
    Buy Now
  </button>
</div>
    </div>
  );
};

export default Cart;
