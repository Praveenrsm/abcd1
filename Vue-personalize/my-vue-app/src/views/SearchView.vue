<template>
  <div class="voice-search">
    <h2>🎤 Voice Translator (Ollama + Qwen)</h2>

    <input ref="searchInput" type="text" v-model="searchText" placeholder="Say something..."
      @keypress.enter="triggerSearch" />

    <div class="btns">
      <button @click="startVoiceSearch">🎙️ Speak</button>
      <button @click="triggerSearch">🔍 Translate</button>
    </div>

    <div v-if="loading" class="loading">Translating... please wait ⏳</div>

    <p v-if="translatedText"><strong>Translated:</strong> {{ translatedText }}</p>
  </div>
</template>

<script>
export default {
  name: "SearchView",
  data() {
    return {
      searchText: '',
      translatedText: '',
      loading: false,
    };
  },
  methods: {
    startVoiceSearch() {
      const SpeechRecognition =
        window.SpeechRecognition || window.webkitSpeechRecognition;
      if (!SpeechRecognition) {
        alert('Your browser does not support voice recognition');
        return;
      }

      const recognition = new SpeechRecognition();
      recognition.lang = 'en-US';
      recognition.interimResults = false;
      recognition.maxAlternatives = 1;

      recognition.start();

      recognition.onresult = (event) => {
        const transcript = event.results[0][0].transcript.trim();
        this.searchText = transcript;
        this.triggerSearch();
      };

      recognition.onerror = (event) => {
        console.error('Voice recognition error:', event.error);
      };

      recognition.onend = () => {
        console.log('Voice recognition ended');
      };
    },

    async triggerSearch() {
      if (!this.searchText) {
        alert('Say or type something first!');
        return;
      }

      this.loading = true;
      this.translatedText = '';

      try {
        const response = await fetch('http://127.0.0.1:11500/v1/chat/completions', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            model: 'qwen3:0.6b', // your model name
            messages: [
              {
                role: 'user',
                content: `
You are a professional translator. Translate the following Kannada text to English script :
"${this.searchText}"
Do not use any other language.
`
              },
            ],
          }),
        });

        const data = await response.json();
        console.log('Ollama response:', data);

        // Chat completion returns `choices[0].message.content`
        this.translatedText =
          data?.choices?.[0]?.message?.content || '(no translation)';
      } catch (err) {
        console.error('Translation error:', err);
        alert('Error connecting to Ollama. Make sure the server is running!');
      } finally {
        this.loading = false;
      }
    },
  },
};
</script>

<style scoped>
.voice-search {
  max-width: 500px;
  margin: 2rem auto;
  padding: 2rem;
  text-align: center;
  border-radius: 16px;
  box-shadow: 0 0 15px rgba(0, 0, 0, 0.1);
  background-color: #fff;
}

.voice-search input {
  width: 100%;
  padding: 10px;
  margin-bottom: 1rem;
  border-radius: 8px;
  border: 1px solid #ccc;
}

.btns {
  display: flex;
  justify-content: center;
  gap: 1rem;
}

button {
  padding: 10px 16px;
  border: none;
  border-radius: 10px;
  background-color: #007bff;
  color: white;
  cursor: pointer;
  transition: 0.3s;
}

button:hover {
  background-color: #0056b3;
}

.loading {
  color: #555;
  margin-top: 1rem;
}
</style>
