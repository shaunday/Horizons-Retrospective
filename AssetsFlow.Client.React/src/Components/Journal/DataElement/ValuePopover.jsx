import React from "react";
import { Textarea, Button, Stack } from "@mantine/core";
import { useForm } from "@mantine/form";
import DefaultSelect from "./DefaultSelect";

function ValuePopover({ contentValue, textRestrictions, onSubmit }) {
  const textRestrictionsExist  =
    Array.isArray(textRestrictions) && textRestrictions.length > 0;

  const form = useForm({
    initialValues: {
      value: contentValue || "",
      changeDetails: "",
    },
    validateInputOnChange: true, 
    validate: {
      value: (val) => {
        const trimmed = val.trim();
        const isEmpty = !trimmed;
        const isRestrictedValid =
          !textRestrictionsExist  || textRestrictions.includes(trimmed);

        if (isEmpty) return "Value cannot be empty";
        if (!isRestrictedValid) return "Value must be from the list";

        return null;
      },
    },
  });

   const handleSubmit = (values) => {
    onSubmit(values.value, values.changeDetails);
    onClose();
  };

  return (
    <form
      onSubmit={form.onSubmit(handleSubmit)}
      onClick={(e) => e.stopPropagation()}
    >
      <Stack spacing="xs" className="w-60">
        {textRestrictionsExist  ? (
          <>
            <DefaultSelect
              value={form.values.value}
              onChange={(val) => form.setFieldValue("value", val)}
              data={textRestrictions}
            />
            {form.errors.value && (
              <div style={{ color: "red", fontSize: 12, marginTop: 4 }}>
                {form.errors.value}
              </div>
            )}
          </>
        ) : (
          <Textarea
            label="Value"
            minRows={1}
            maxRows={3}
            autosize
            placeholder="Enter value"
            autoFocus
            {...form.getInputProps("value")}
            error={form.errors.value}
          />
        )}

        <Textarea
          label="Change notes"
          minRows={2}
          {...form.getInputProps("changeDetails")}
        />

        <Button type="submit" size="xs" disabled={!form.isValid()}>
          Save
        </Button>
      </Stack>
    </form>
  );
}

export default React.memo(ValuePopover);
