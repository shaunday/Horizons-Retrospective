
// Function to create the structure for the labeled field info
export const createLabeledFieldInfoStruct = (id, type, name, value = "") => {
  return { id, type, name, value };
};

function LabeledField({ info, onChange }) {
  return (
    <div style={{ marginBottom: 10 }}>
      <label>{info.title}</label>
      <input
        type={info.type}
        name={info.id}  
        value={info.value}
        onChange={onChange}
        aria-label={info.title}
        style={{ marginLeft: 5 }}
      />
    </div>
  );
}

export default React.memo(LabeledField);
