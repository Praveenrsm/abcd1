import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Header from "./Component/Header";
import Admin from './admin';
import ProductDetails from './Product/ProductDetails';
import Login from './User/Login';
import Products from './Product/Products';
import AddProduct from './Product/NewProduct';
import ForgotPassword from './User/ForgotPassword';
import ResetPassword from './User/ResetPassword';
import Register from './User/Register';
import GetAllCart from './cart/GetAllCart';
import SelectAddress from './Address/SelectAddress';
import OrderDetails from './Order/OrderDetails';
import AddAddress from './Address/AddAddress';
import './User/Login.css';
import './User/Register.css';
import './User/ForgotPassword.css';
import './User/ResetPassword.css';
import './Product/Products.css';
import './Product/ProductDetails.css';
import './Product/NewProduct.css';
import './cart/GetAllCart.css';
import './Component/Header.css';
import './Address/SelectAddress.css';
import './Order/OrderDetails.css';
const App = () => {
    return (
        <Router>
            <Header /> 
            <Routes>
                <Route path="/" element={<Login />} />
                <Route path="/admin" element={<Admin />} />
                <Route path="/Product/ProductDetails/:productId" element={<ProductDetails />} />
                <Route path="/NewProduct" element={<AddProduct />} />
                <Route path="/Products" element={<Products />} />
                <Route path="/ForgotPassword" element={<ForgotPassword />} />
                <Route path="/ResetPassword" element={<ResetPassword />} />
                <Route path="/Register" element={<Register />} />
                <Route path="/GetAllcart" element={<GetAllCart />} />
                <Route path="/SelectAddress" element={<SelectAddress />} />
                <Route path="/OrderDetails/:orderId" element={<OrderDetails />} />
                <Route path="/AddAddress" element={<AddAddress />} />
            </Routes>
        </Router>
    );
};

export default App;
