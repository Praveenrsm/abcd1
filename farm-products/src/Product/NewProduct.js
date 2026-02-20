import React, { useState } from 'react';
import axios from 'axios';
const API_URL = process.env.REACT_APP_API_URL;
function AddProduct() {
    const [ProductName, setProductName] = useState('');
    const [productPrice, setProductPrice] = useState(0);
    const [Description, setDescription] = useState('');
    const [availableQuantity, setAvailableQuantity] = useState(0);
    const [images, setImages] = useState([]);
const payload = {
  product: {
    productName: ProductName,
    productPrice: Number(productPrice),
    description: Description,
    availableQuantity: Number(availableQuantity)
  },
  imageList: images
};
    const handleImageUpload = (event) => {
        const files = event.target.files;
        const imageArray = Array.from(files);

         //Convert each image to base64 string
        const promises = imageArray.map((file) => {
            return new Promise((resolve, reject) => {
               const reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = () => resolve(reader.result.split(',')[1]); // Remove "data:image/png;base64,"
               reader.onerror = (error) => reject(error);
            });
        });

        Promise.all(promises)
           .then(base64Images => setImages(base64Images))
           .catch(error => console.error("Error uploading images:", error));
    };

    const handleSubmit = async (event) => {
        event.preventDefault();

        try {
            const response = await axios.post(`${API_URL}/api/Products/AddProduct`, payload);
            console.log(response.data);
            alert("Product added successfully!");
        } catch (error) {
            console.error("Error adding product:", error);
            alert("Failed to add product: " + (error.response?.data?.title || error.message)); // Enhanced error message
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <div>
                <label>Product Name:</label>
                <input type="text" value={ProductName} onChange={(e) => setProductName(e.target.value)} required />
            </div>
            <div>
                <label>Product Price:</label>
                <input type="number" value={productPrice} onChange={(e) => setProductPrice(e.target.value)} required />
            </div>
            <div>
                <label>Description:</label>
                <textarea value={Description} onChange={(e) => setDescription(e.target.value)} required />
            </div>
            <div>
                <label>Available Quantity:</label>
                <input type="number" value={availableQuantity} onChange={(e) => setAvailableQuantity(e.target.value)} required />
            </div>
            <div>
                <label>Upload Images:</label>
                <input type="file" multiple onChange={handleImageUpload} accept="image/*" />
            </div>
            <button type="submit">Add Product</button>
        </form>
    );
}

export default AddProduct;
