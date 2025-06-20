import React from "react";

export const createLabeledFieldInfoStruct = (id, type, name, value = "") => {
  return { id, type, name, value };
};

function LabeledField({ info, onChange }) {
  return (
    <div className="mb-2.5">
      <label>{info.title}</label>
      <input
        type={info.type}
        name={info.id}  
        value={info.value}
        onChange={onChange}
        aria-label={info.title}
        className="ml-1"
      />
    </div>
  );
}

export default React.memo(LabeledField);
