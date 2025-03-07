import { useState, useRef, useEffect } from "react";

function useHover() {
  const [isHovered, setIsHovered] = useState(false);
  const ref = useRef(null);

  useEffect(() => {
    const handlePointerMove = (event) => {
      if (isHovered && ref.current && !ref.current.contains(event.target)) {
        setIsHovered(false);
      }
    };

    document.addEventListener("pointermove", handlePointerMove);
    return () => document.removeEventListener("pointermove", handlePointerMove);
  }, [isHovered]);

  return { ref, isHovered, setIsHovered };
}

export default useHover;
