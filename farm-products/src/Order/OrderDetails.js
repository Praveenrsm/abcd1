import React, { useEffect, useState } from "react";
import axios from "axios";
import { useParams, useNavigate } from "react-router-dom";

const OrderDetails = () => {
  const { orderId } = useParams();
  const [order, setOrder] = useState(null);
const [loading, setLoading] = useState(true);
const [notFound, setNotFound] = useState(false);

  const token = localStorage.getItem("token");
  const navigate = useNavigate();
  useEffect(() => {
    fetchOrder();
  }, []);
 const cancelOrder = async () => {
    if (!window.confirm("Are you sure you want to cancel this order?")) return;

    await axios.put(
      "https://localhost:7292/api/Order/CancelOrder",
      null,
      {
        params: { orderId },
        headers: { Authorization: `Bearer ${token}` }
      }
    );

    alert("Order cancelled");
    navigate(`/OrderDetails/${orderId}`); // order details page
  };
  const fetchOrder = async () => {
  try {
    const res = await axios.get(
      "https://localhost:7292/api/Order/GetOrderDetails",
      {
        params: { orderId },
        headers: { Authorization: `Bearer ${token}` }
      }
    );

    if (res.data === null) {
      setNotFound(true);
    } else {
      setOrder(res.data);
    }
  } catch (err) {
    setNotFound(true);
  } finally {
    setLoading(false);
  }
};

  if (loading) return <p>Loading...</p>;
  if (notFound) return <p>Order not found</p>;

  return (
    <div className="order-details">
      <h2>Order #{order.orderId}</h2>
      <p>Status: {order.status}</p>

      {order.status === "confirmed" && (
        <button onClick={cancelOrder} style={{ background: "red", color: "white" }}>
          Cancel Order
        </button>
      )}
      <p>Order Date: {new Date(order.orderDate).toLocaleString()}</p>

      <h3>Delivery Address</h3>
      <p>{order.address.street}</p>
      <p>
        {order.address.city}, {order.address.state} - {order.address.pinCode}
      </p>

      <h3>Items</h3>
      {order.items.map((item, index) => (
        <div key={index} className="order-item">
          <p><b>{item.productName}</b></p>
          <p>Qty: {item.quantity}</p>
          <p>Unit Price: ₹{item.unitPrice}</p>
          <p>Total: ₹{item.totalPrice}</p>
        </div>
      ))}

      <h3>Grand Total: ₹{order.grandTotal}</h3>
    </div>
  );
};

export default OrderDetails;
