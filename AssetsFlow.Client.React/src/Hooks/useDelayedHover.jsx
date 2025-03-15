import { useState, useRef, useEffect } from "react";

export function useDelayedHover(delay = 150) {
  const ref = useRef(null);
  const [delayedHover, setDelayedHover] = useState(false);
  let timer = useRef(null);

  useEffect(() => {
    const handleMouseEnter = () => {
      clearTimeout(timer.current);
      timer.current = setTimeout(() => setDelayedHover(true), delay);
    };

    const handleMouseLeave = () => {
      clearTimeout(timer.current);
      setDelayedHover(false);
    };

    const element = ref.current;
    if (element) {
      element.addEventListener("mouseenter", handleMouseEnter);
      element.addEventListener("mouseleave", handleMouseLeave);
    }

    return () => {
      clearTimeout(timer.current);
      if (element) {
        element.removeEventListener("mouseenter", handleMouseEnter);
        element.removeEventListener("mouseleave", handleMouseLeave);
      }
    };
  }, [delay]);

  return { delayedHover, ref };
}