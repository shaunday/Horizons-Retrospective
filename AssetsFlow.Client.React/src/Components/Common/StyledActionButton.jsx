import React from "react";
import { Button } from "@mantine/core";

function StyledActionButton({ 
  children, 
  icon, 
  onClick, 
  type = "button", 
  disabled = false, 
  className = "",
  minWidth = "200px"
}) {
  return (
    <Button
      type={type}
      leftSection={icon}
      variant="subtle"
      color="blue"
      size="sm"
      disabled={disabled}
      onClick={onClick}
      className={`shadow-md hover:shadow-lg transition-all duration-200 hover:scale-105 ${className}`}
      style={{ minWidth }}
    >
      {children}
    </Button>
  );
}

export default StyledActionButton; 