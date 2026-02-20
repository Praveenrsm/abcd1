import { Link } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";

const Header = () => {
  const [cartCount, setCartCount] = useState(0);

  useEffect(() => {
    const fetchCartCount = async () => {
      try {
        const token = localStorage.getItem("token");
        const res = await axios.get(
          "https://localhost:7292/api/Cart/GetCart",
          {
            headers: token
              ? { Authorization: `Bearer ${token}` }
              : {}
          }
        );
        setCartCount(res.data.length);
      } catch {
        setCartCount(0);
      }
    };

    fetchCartCount();
  }, []);

  return (
    <header className="header">

      <nav>
        <Link to="/GetAllcart">🛒 Cart ({cartCount})</Link>
        <Link to="/">Login</Link>
      </nav>
    </header>
  );
};

export default Header;
