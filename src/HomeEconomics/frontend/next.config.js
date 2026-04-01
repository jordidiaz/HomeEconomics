
/** @type {import('next').NextConfig} */
const nextConfig = {
  output: "export",
  reactStrictMode: true,
  async rewrites() {
    if (process.env.NODE_ENV !== "development") {
      return [];
    }
    const port =5050;
    return [
      {
        source: '/api/:path*',
        destination: `http://localhost:${port}/api/:path*`,
      },
    ];
  },
};

module.exports = nextConfig;
