import "./globals.css";
import {Metadata} from "next";

export const metadata: Metadata = {
    title: "Todoododoo",
    description: "A tiny to-do list application",
};

export default function RootLayout({
                                       children,
                                   }: Readonly<{
    children: React.ReactNode;
}>) {

    return (
        <html lang="en">
        <body>
        <div className="flex">
            <main className="w-full flex justify-center">
                {children}
            </main>
        </div>
        </body>
        </html>
    );
}
