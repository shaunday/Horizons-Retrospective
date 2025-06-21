# CLSX Usage Examples

This document shows how to use `clsx` for conditional CSS classes in your React components.

## Basic Usage

```jsx
import clsx from "clsx";

// Simple conditional class
<div className={clsx("base-class", {
  "conditional-class": someCondition
})}>

// Multiple conditions
<div className={clsx("base-class", {
  "class-a": conditionA,
  "class-b": conditionB,
  "class-c": conditionC
})}>
```

## Before vs After Examples

### Before (Template Literals)
```jsx
// Complex template literal
<div className={`mr-1 flex ${isCollapsed ? 'flex-row' : 'flex-col'} gap-1`}>

// Multiple ternaries
<div className={`container px-1 ${isOverview ? 'max-w-60' : 'max-w-36'} min-w-18 flex gap-1 ${flexClass} ${borderClass}`}>
```

### After (CLSX)
```jsx
// Clean and readable
<div className={clsx("mr-1 flex gap-1", {
  "flex-row": isCollapsed,
  "flex-col": !isCollapsed
})}>

// All conditions in one place
<div className={clsx("container px-1 min-w-18 flex gap-1", {
  "max-w-60": isOverview,
  "max-w-36": !isOverview,
  "flex-row": isOverview,
  "flex-col": !isOverview,
  "border border-red-500": hasError
})}>
```

## Advanced Patterns

### Arrays
```jsx
// Combine arrays of classes
const baseClasses = ["flex", "gap-2"];
const conditionalClasses = ["p-4", "rounded"];

<div className={clsx(baseClasses, conditionalClasses, {
  "bg-blue-500": isActive
})}>
```

### Functions
```jsx
// Dynamic class generation
const getSizeClass = (size) => {
  return clsx({
    "text-sm": size === "small",
    "text-base": size === "medium", 
    "text-lg": size === "large"
  });
};

<div className={clsx("font-bold", getSizeClass(size))}>
```

### Multiple Objects
```jsx
// Combine multiple conditional objects
const sizeClasses = {
  "text-sm": size === "small",
  "text-lg": size === "large"
};

const stateClasses = {
  "opacity-50": disabled,
  "cursor-pointer": !disabled
};

<div className={clsx("base-class", sizeClasses, stateClasses)}>
```

## Benefits

1. **Readability**: Clear separation of base and conditional classes
2. **Maintainability**: Easier to add/remove conditions
3. **Performance**: Slightly better than template literals
4. **Type Safety**: Better IDE support and error detection
5. **Flexibility**: Supports strings, objects, arrays, and functions

## Migration Tips

1. Start with the most complex conditional classes
2. Replace template literals with `clsx` objects
3. Remove unused variables that were storing class strings
4. Use object syntax for boolean conditions
5. Keep base classes as the first argument 