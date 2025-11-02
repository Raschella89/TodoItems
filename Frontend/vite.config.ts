import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";

export default defineConfig({
  plugins: [vue()],
  server: {
    port: 5173, // frontend port
    proxy: {
      "/api": "http://localhost:5101", // redirect API calls to backend
    },
  },
});
