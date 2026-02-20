import React, { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const SelectAddress = () => {
  const [addresses, setAddresses] = useState([]);
  const [selectedAddress, setSelectedAddress] = useState(null);
  const navigate = useNavigate();
  const token = localStorage.getItem("token");

  useEffect(() => {
    fetchAddresses();
  }, []);

  const fetchAddresses = async () => {
    const res = await axios.get(
      "https://localhost:7292/api/Address/GetAddresss",
      { headers: { Authorization: `Bearer ${token}` } }
    );
    setAddresses(res.data);
  };

  const placeOrder = async () => {
    if (!selectedAddress) {
      alert("Please select address");
      return;
    }

    try {
      const res = await axios.post(
        "https://localhost:7292/api/Order/PlaceOrder",
        null,
        {
          params: { adddress: selectedAddress },
          headers: { Authorization: `Bearer ${token}` }
        }
      );

      const orderId = res.data.orderId; // ✅ backend response

      navigate(`/orderdetails/${orderId}`);
    } catch (err) {
      alert("Order failed");
    }
  };

  return (
    <div className="address-page">
      <h2>Select Delivery Address</h2>

      {addresses.map(addr => (
        <div key={addr.id} className="address-card">
          <input
            type="radio"
            name="address"
            onChange={() => setSelectedAddress(addr.id)}
          />
          <div>
            <p><b>{addr.street}</b></p>
            <p>{addr.city}, {addr.state} - {addr.pinCode}</p>
            <p>{addr.country}</p>
            <p>Phone: {addr.phone}</p>
          </div>
        </div>
      ))}
      <button
        className="buy-now-btn"
        onClick={() => navigate("/AddAddress")}
      >
        Add Address
      </button>
      <br />
      <br />
      <button onClick={placeOrder}>
        Place Order (Cash on Delivery)
      </button>
    </div>
  );
};

export default SelectAddress;
