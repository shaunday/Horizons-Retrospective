import React from "react";
import Header from "@views/Header";
import Footer from "@views/Footer";

import PropTypes from "prop-types";

function Layout({ children, showUserActions = true }) {
  return (
    <div className="h-full flex flex-col">
      <Header showUserActions={showUserActions} />
      <main className="flex-1 overflow-y-auto">{children}</main>
      <Footer />
    </div>
  );
}

// Prop types check
Layout.propTypes = {
  children: PropTypes.node.isRequired,
  showUserActions: PropTypes.bool,
};

// Memoize
export default React.memo(Layout);
