import React from "react";
import { TextInput, Textarea, Button, Stack } from "@mantine/core";
import { useForm } from "@mantine/form";
import DefaultSelect from "./DefaultSelect";

function ValuePopover({ contentValue, textRestrictions, onSubmit }) {
  const textRestrictionsExist = textRestrictions?.length > 0;

  const form = useForm({
    initialValues: {
      value: contentValue || "",
      changeDetails: "",
    },
    validate: {
      value: (val) => {
        if (val.trim() === "") return "Value cannot be empty";
        if (textRestrictionsExist && !textRestrictions.includes(val))
          return "Selected value is not allowed";
        return null;
      },
    },
  });

  const handleSubmit = (values) => {
    onSubmit(values.value, values.changeDetails);
  };

  return (
    <form
      onSubmit={form.onSubmit(handleSubmit)}
      onClick={(e) => e.stopPropagation()}
    >
      <Stack spacing="xs">
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
            placeholder="Enter value"
            autoFocus
          />
        )}

        <Textarea
          label="Change notes"
          minRows={2}
          {...form.getInputProps("changeDetails")}
        />

        <Button
          type="submit"
          size="xs"
          variant="filled"
          disabled={!form.isValid()}
        >
          Save
        </Button>
      </Stack>
    </form>
  );
}

export default React.memo(ValuePopover);
