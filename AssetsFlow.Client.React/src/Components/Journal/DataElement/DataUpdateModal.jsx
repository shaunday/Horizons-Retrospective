import React from 'react';
import { Modal, TextInput, Button } from '@mantine/core';
import { useForm } from '@mantine/form';
import * as Constants from "@constants/journalConstants";
import { dataElementContentParser } from "@services/dataElementContentParser";

function DataUpdateModal({ opened, onClose, onSubmit, data }) {
    const { contentValue } = dataElementContentParser(data);
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
        <Modal size="lg" opened={opened} onClose={onClose} title={data[Constants.DATA_TITLE_STRING]} centered>
            <form onSubmit={form.onSubmit(handleSubmit)}>
                <TextInput
                    label="Value"
                    {...form.getInputProps('value')}
                    mb={10}
                />
                <TextInput
                    label="Change Details"
                    mb={20}
                />
                <Button type="submit" style={{ display: 'block', margin: '0 auto' }}>
                    Apply Changes
                </Button>
            </form>
        </Modal>
    );
}

export default DataUpdateModal;
