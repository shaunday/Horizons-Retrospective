import React from "react";
import Header from "@views/Header";
import Footer from "@views/Footer";

export default function Layout({ children, showUserActions = true }) {
  return (
    <div className="min-h-screen flex flex-col">
      <Header showUserActions={showUserActions} />
      <main className="flex-1 overflow-y-auto">{children}</main>
      <Footer />
    </div>
  );
}
