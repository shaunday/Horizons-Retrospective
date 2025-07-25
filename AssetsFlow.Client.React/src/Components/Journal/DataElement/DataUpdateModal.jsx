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

  const isRestrictedValid =
    !textRestrictionsExist || textRestrictions.includes(form.values.value);

  return (
    <Modal
      size="lg"
      centered
      opened={opened}
      onClose={onClose}
      title={
        <div className="text-lg font-semibold text-gray-700 text-center">
          {data[Constants.DATA_TITLE_STRING]}
        </div>
      }
      onClick={handleModalClick}
    >
      <form
        onSubmit={form.onSubmit(handleSubmit)}
        onClick={handleModalClick}
        className="space-y-4"
      >
        {textRestrictionsExist ? (
          <DefaultSelect
            value={form.values.value}
            onChange={(val) => form.setFieldValue("value", val)}
            data={textRestrictions}
          />
        ) : (
          <Textarea
            autosize
            maxRows={2}
            label="Value"
            {...form.getInputProps("value")}
          />
        )}

        <Textarea
          autosize
          maxRows={2}
          label="Change Details"
          {...form.getInputProps("changeDetails")}
        />

        <div className="flex justify-center">
          <StyledActionButton
            type="submit"
            icon={<TbCheck size={20} />}
            disabled={!isRestrictedValid}
            onClick={handleModalClick}
          >
            Apply Changes
          </StyledActionButton>
        </div>
      </form>
    </Modal>
  );
}

export default DataUpdateModal;
