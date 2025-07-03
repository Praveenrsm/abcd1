<template>
  <div>
    <h2>Contact Us</h2>
    <form @submit.prevent="submitForm">
      <label>Name:</label>
      <input type="text" v-model="formData.name" @input="saveFormData" />
      <span v-if="errors.name" class="error">{{ errors.name }}</span><br />

      <label>Age:</label>
      <input type="number" v-model="formData.age" @input="saveFormData" />
      <span v-if="errors.age" class="error">{{ errors.age }}</span><br />

      <label>Gender:</label>
      <select v-model="formData.gender" @change="saveFormData">
        <option value="">Select</option>
        <option value="Male">Male</option>
        <option value="Female">Female</option>
        <option value="Other">Other</option>
      </select>
      <span v-if="errors.gender" class="error">{{ errors.gender }}</span><br />

      <label>Message:</label>
      <textarea v-model="formData.message" @input="saveFormData"></textarea>
      <span v-if="errors.message" class="error">{{ errors.message }}</span><br />

      <label>Contact:</label>
      <input type="text" v-model="formData.contact" @input="saveFormData" />
      <span v-if="errors.contact" class="error">{{ errors.contact }}</span><br />

      <label>Email:</label>
      <input type="email" v-model="formData.email" @input="saveFormData" />
      <span v-if="errors.email" class="error">{{ errors.email }}</span><br />

      <button type="submit" :disabled="isSubmitted">Submit</button>
    </form>
  </div>
</template>

<script>
export default {
  data() {
    return {
      formData: {
        name: "",
        age: "",
        gender: "",
        message: "",
        contact: "",
        email: "",
      },
      errors: {}, // Store validation errors
      isSubmitted: false,
      expiryTime: 24 * 60 * 60 * 1000, // 24 hours in milliseconds
    };
  },
  methods: {
    saveFormData() {
      if (!this.isSubmitted) {
        const dataToSave = {
          formData: this.formData,
          timestamp: Date.now(), // Save current timestamp
        };
        localStorage.setItem("contactFormData", JSON.stringify(dataToSave));
      }
    },
    loadFormData() {
      const savedData = localStorage.getItem("contactFormData");
      if (savedData) {
        const parsedData = JSON.parse(savedData);
        const currentTime = Date.now();

        if (parsedData.timestamp && currentTime - parsedData.timestamp < this.expiryTime) {
          this.formData = parsedData.formData; // Load form data if not expired
        } else {
          localStorage.removeItem("contactFormData"); // Remove expired data
        }
      }
    },
    validateForm() {
      this.errors = {}; // Reset errors

      if (!this.formData.name) this.errors.name = "Please enter your name.";
      if (!this.formData.age) this.errors.age = "Please enter your age.";
      if (!this.formData.gender) this.errors.gender = "Please select your gender.";
      if (!this.formData.message) this.errors.message = "Please enter a message.";
      if (!this.formData.contact) this.errors.contact = "Please enter your contact number.";
      if (!this.formData.email) {
        this.errors.email = "Please enter your email.";
      } else if (!/\S+@\S+\.\S+/.test(this.formData.email)) {
        this.errors.email = "Please enter a valid email.";
      }

      return Object.keys(this.errors).length === 0; // Return true if no errors
    },
    submitForm() {
      if (!this.validateForm()) {
        return; // Stop submission if validation fails
      }

      alert("Form submitted successfully!");
      localStorage.setItem("submitted", "true");

      // Clear stored data after submission
      localStorage.removeItem("contactFormData");
      localStorage.removeItem("submitted");

      // Reset form and disable submission button
      this.isSubmitted = true;
      this.formData = {
        name: "",
        age: "",
        gender: "",
        message: "",
        contact: "",
        email: "",
      };
    },
  },
  mounted() {
    this.loadFormData();
    this.isSubmitted = localStorage.getItem("submitted") === "true";
  },
};
</script>

<style>
.error {
  color: red;
  font-size: 14px;
}
</style>
