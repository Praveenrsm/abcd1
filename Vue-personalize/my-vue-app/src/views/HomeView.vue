<template>
  <div>
    <h1>Welcome to Home Page</h1>

    <div v-if="showPopup" class="popup">
      <p>Hey! You started filling out our Contact Us form.</p>
      <p>Need any help? Complete it now and we'll get back to you soon!</p>
      <button @click="goToContactUs">Go to Contact Form</button>
      <button @click="closePopup">Close</button>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      showPopup: false,
    };
  },
  methods: {
    checkFormStatus() {
      const saved = localStorage.getItem("contactFormData");
      const submitted = localStorage.getItem("submitted");

      // Show popup only if form is started but not submitted
      if (saved && !submitted) {
        this.showPopup = true;
      }
    },
    goToContactUs() {
      this.$router.push("/contact-us");
    },
    closePopup() {
      this.showPopup = false;
      localStorage.removeItem("contactFormData");
      localStorage.removeItem("submitted");
    },
  },
  mounted() {
    this.checkFormStatus();
  },
};
</script>

<style>
.popup {
  position: fixed;
  bottom: 20px;
  right: 20px;
  background: white;
  padding: 15px;
  box-shadow: 0px 0px 10px gray;
  border-radius: 5px;
  z-index: 999;
}
</style>
