'use client';
import "./globals.css";
import Header from "@/components/Header";

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
          <Header/>
          <main className='w-full flex justify-center'>
              {children}
          </main>
      </body>
    </html>
  );
}
