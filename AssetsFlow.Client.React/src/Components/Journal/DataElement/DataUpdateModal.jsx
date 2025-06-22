import React from "react";
import { Modal, Textarea, Button } from "@mantine/core";
import { useForm } from "@mantine/form";
import { TbCheck } from "react-icons/tb";
import * as Constants from "@constants/journalConstants";
import { dataElementContentParser } from "@services/dataElementContentParser";
import DefaultSelect from "./DefaultSelect";
import StyledActionButton from "@components/Common/StyledActionButton";

function DataUpdateModal({ opened, onClose, onSubmit, data }) {
  const { contentValue, textRestrictions } = dataElementContentParser(data);
  const textRestrictionsExist = textRestrictions?.length > 0;

  const form = useForm({
    initialValues: {
      value: contentValue || "",
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

  const handleModalClick = (e) => {
    e.stopPropagation();
  };

  // Check if the current value is allowed if there are restrictions
  const isRestrictedValid =
    !textRestrictionsExist || textRestrictions.includes(form.values.value);

  return (
    <Modal 
      size="lg" 
      centered
      opened={opened}
      onClose={onClose}
      title={data[Constants.DATA_TITLE_STRING]}
      onClick={handleModalClick}
      styles={{
        title: {
          fontSize: '18px',
          fontWeight: '600',
          color: '#374151',
          textAlign: 'center',
        }
      }}
    >
      <form onSubmit={form.onSubmit(handleSubmit)} onClick={handleModalClick}>
        {textRestrictionsExist ? (
          <DefaultSelect
            value={form.values.value}
            onChange={(val) => form.setFieldValue("value", val)}
            data={textRestrictions}
          />
        ) : (
          <Textarea autosize maxRows={2}
            label="Value"
            className="mb-5"
            {...form.getInputProps("value")}
          />
        )}

        <Textarea autosize maxRows={2}
          label="Change Details"
          className="mb-5 mt-4"
          {...form.getInputProps("changeDetails")}
        />

        <StyledActionButton
          type="submit"
          icon={<TbCheck size={20} />}
          disabled={!isRestrictedValid}
          className="block mx-auto"
          onClick={handleModalClick}
        >
          Apply Changes
        </StyledActionButton>
      </form>
    </Modal>
  );
}

export default DataUpdateModal;
