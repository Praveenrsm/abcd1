<template>
  <div class="p-4">
    <h2>🧠 Semantic Search (Vue + Ollama + Qdrant)</h2>

    <input v-model="query" placeholder="Search product..." class="border p-2" />
    <button @click="onSearch" class="ml-2 bg-blue-500 text-white p-2 rounded">Search</button>

    <div class="mt-4">
      <h3>Results:</h3>
      <ul>
        <li v-for="(item, i) in results" :key="i">
           {{ item.name }} <small>score: {{ item.score }}</small> 
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { addProduct, searchProducts } from "../services/SemanticService.js";

const query = ref("");
const results = ref([]);

// 🌱 add sample products on mount
onMounted(async () => {
  await addProduct({ id: 1, name: "skin care" });
  await addProduct({ id: 2, name: "hair shampoo" });
  await addProduct({ id: 3, name: "face wash" });
  await addProduct({ id: 4, name: "gentle face cleanser" });
  await addProduct({ id: 5, name: "oil control face wash" });
  await addProduct({ id: 6, name: "skin wash" });
  await addProduct({ id: 7, name: "hair dye" });
  await addProduct({ id: 8, name: "face cream" });
  await addProduct({ id: 9, name: "men face shampoo" });
  await addProduct({ id: 10, name: "dads oil control face wash" });
});

const onSearch = async () => {
  results.value = await searchProducts(query.value,0.55);
};
</script>
