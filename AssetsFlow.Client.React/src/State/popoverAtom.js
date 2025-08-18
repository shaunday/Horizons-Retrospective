import { atom } from "jotai";

// Stores the unique ID of the currently opened popover, or null if none
export const openPopoverIdAtom = atom(null);
