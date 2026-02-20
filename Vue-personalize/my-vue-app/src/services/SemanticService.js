import axios from "axios";

const QDRANT_URL = "http://127.0.0.1:6333";
const OLLAMA_URL = "http://127.0.0.1:11434";

// 1. Get embeddings from Ollama
export async function getEmbedding(text) {
  try {
    const res = await axios.post(`${OLLAMA_URL}/api/embed`, {
      model: "nomic-embed-text",
      input: text
    });
    console.log("Embedding response:", res.data);
    return res.data.embeddings[0];
  } catch (err) {
    console.error("Ollama embedding error:", err.response?.data || err);
    throw err;
  }
}

// 2. Ensure collection exists before adding
async function ensureCollection(size = 768) {
  try {
    await axios.put(`${QDRANT_URL}/collections/products`, {
      vectors: { size, distance: "Cosine" }
    });
    console.log("✅ Qdrant collection created");
  } catch (err) {
    if (err.response?.status === 409) {
      console.log("ℹ️ Collection already exists, continuing...");
      return;
    }
    console.error("❌ Qdrant collection creation failed:", err.response?.data || err);
    throw err;
  }
}

// 📦 3. Add a product to Qdrant
export async function addProduct(product) {
  try {
    const embedding = await getEmbedding(product.name);

    // Make sure collection exists first
    await ensureCollection(embedding.length);

    const point = {
      points: [
        {
          id: product.id,
          vector: embedding,
          payload: { name: product.name }
        }
      ]
    };

    const res = await axios.put(`${QDRANT_URL}/collections/products/points`, point);
    console.log("✅ Product added:", res.data);
  } catch (err) {
    console.error("❌ Qdrant addProduct error:", err.response?.data || err);
  }
}

// 🔍 4. Search similar products
export async function searchProducts(query, threshold = 0.55) {
  try {
    const embedding = await getEmbedding(query);

    const payload = {
      vector: embedding,
      limit: 5,       // return top 5
      with_payload: true,
      with_vector: false
    };

    const res = await axios.post(
      `${QDRANT_URL}/collections/products/points/search`,
      payload
    );

    console.log("🔍 Raw search result:", res.data);

    // Filter safely, allow low-score results
    const results = res.data.result
      ?.filter(r => r.payload && r.payload.name && r.score >= threshold)
      .map(r => ({
        name: r.payload.name,
        score: r.score.toFixed(3) // show similarity score
      })) || [];

    if (results.length === 0)
      console.warn("⚠️ No similar results found for:", query);

    return results;
  } catch (err) {
    console.error("❌ Qdrant search error:", err.response?.data || err);
    return [];
  }
}
