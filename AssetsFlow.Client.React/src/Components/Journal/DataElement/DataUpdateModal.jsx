import React from "react";
import { Modal, Textarea, Button } from "@mantine/core";
import { useForm } from "@mantine/form";
import * as Constants from "@constants/journalConstants";
import { dataElementContentParser } from "@services/dataElementContentParser";
import DefaultSelect from "./DefaultSelect";

function DataUpdateModal({ opened, onClose, onSubmit, data }) {
  const { contentValue, textRestrictions } = dataElementContentParser(data);
  const textRestrictionsExist = textRestrictions?.length > 0;

  const form = useForm({
    initialValues: {
      value: contentValue || "", // Ensure value initializes correctly
      changeDetails: "",
    },
    validate: {
      value: (val) => (val.trim() === "" ? "Value cannot be empty" : null),
    },
  });

  const handleSubmit = (values) => {
    onSubmit(values.value, values.changeDetails);
    onClose();
  };

  return (
    <Modal size="lg" opened={opened} onClose={onClose} title={data[Constants.DATA_TITLE_STRING]} centered>
      <form onSubmit={form.onSubmit(handleSubmit)}>
        {textRestrictionsExist ? (
          <DefaultSelect
            value={form.values.value}
            onChange={(val) => form.setFieldValue("value", val)}
            data={textRestrictions}
          />
        ) : (
          <Textarea label="Value" {...form.getInputProps("value")} mb={10} autosize maxRows={2}/>
        )}

        <Textarea label="Change Details" {...form.getInputProps("changeDetails")} mb={20} autosize maxRows={2}/>

        <Button type="submit" style={{ display: "block", margin: "0 auto" }}>
          Apply Changes
        </Button>
      </form>
    </Modal>
  );
}

export default DataUpdateModal;