import React from 'react';
import { Modal, NumberInput, Button } from '@mantine/core';
import { useForm } from '@mantine/form';

function TradeClosingModal({ opened, onClose, onSubmit}) {
    const form = useForm({
        initialValues: {
            closePrice: ''
        },
        validate: {
            closePrice: (val) => (val.trim() === '' ? 'Closing Price cannot be empty' : null),
        },
    });

    const handleSubmit = (values) => {
        const { closePrice } = values;
        onSubmit(closePrice);
        onClose();
    };

    return (
        <Modal opened={opened} onClose={onClose} title="Enter closing price" centered>
            <form onSubmit={form.onSubmit(handleSubmit)}>
                <NumberInput
                    label="Closing Price:"
                    {...form.getInputProps('closePrice')}
                    mb={5}
                />
                <Button type="submit" style={{ display: 'block', margin: '0 auto' }}>
                    Apply Changes
                </Button>
            </form>
        </Modal>
    );
}

export default TradeClosingModal;
