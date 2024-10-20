import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";
import path from "path";
import Pages from "vite-plugin-pages";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    react(),
    Pages({
      dirs: [{ dir: "src/routes", baseRoute: "" }],
      exclude: ["**/components/*.tsx"],
    }),
  ],
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src"),
    },
  },
});
