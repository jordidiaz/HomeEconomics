import type { Metadata } from "next";
import type { ReactNode } from "react";
import { AppThemeProvider } from "../components/app-theme-provider";

export const metadata: Metadata = {
  title: "HomeEconomics",
  description: "HomeEconomics frontend bootstrap",
};

type RootLayoutProps = {
  children: ReactNode;
};

export default function RootLayout({ children }: RootLayoutProps) {
  return (
    <html lang="en">
      <body>
        <AppThemeProvider>{children}</AppThemeProvider>
      </body>
    </html>
  );
}
