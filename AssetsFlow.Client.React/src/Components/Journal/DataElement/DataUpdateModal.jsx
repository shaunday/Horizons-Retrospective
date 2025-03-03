import React from 'react';
import { Modal, TextInput, Button } from '@mantine/core';
import { useForm } from '@mantine/form';
import * as Constants from "@constants/journalConstants";
import { dataParser } from "./dataParser";

function DataUpdateModal({ opened, onClose, onSubmit, data }) {
    const { contentValue } = dataParser(data);
    const form = useForm({
        initialValues: {
            value: contentValue || ''
        },
        validate: {
            value: (val) => (val.trim() === '' ? 'Value cannot be empty' : null),
        },
    });

    const handleSubmit = (values) => {
        const { value, changeDetails } = values;
        onSubmit(value, changeDetails);
        onClose();
    };

    return (
        <Modal opened={opened} onClose={onClose} title={`Edit: ${data[Constants.DATA_TITLE_STRING]}`} centered>
            <form onSubmit={form.onSubmit(handleSubmit)}>
                <TextInput
                    label="Value:"
                    {...form.getInputProps('value')}
                    mb={5}
                />
                <TextInput
                    label="Change Details:"
                    mb={15}
                />
                <Button type="submit" style={{ display: 'block', margin: '0 auto' }}>
                    Apply Changes
                </Button>
            </form>
        </Modal>
    );
}

export default DataUpdateModal;
