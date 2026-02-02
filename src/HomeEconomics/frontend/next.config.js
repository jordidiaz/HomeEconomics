
/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  async rewrites() {
    const port = 5000; // development server port
    // const port = 6001; // production server port
    return [
      {
        source: '/api/:path*',
        destination: `http://localhost:${port}/api/:path*`,
      },
    ];
  },
};

module.exports = nextConfig;
