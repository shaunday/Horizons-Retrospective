import { Modal, NumberInput, Button } from '@mantine/core';
import { useForm } from '@mantine/form';

function TradeClosingModal({ opened, onClose, onSubmit }) {
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
                    placeholder="Closing price..."
                    thousandSeparator=","
                    hideControls
                    {...form.getInputProps('closePrice')}
                    className="mb-1 max-w-xs block mx-auto"
                />
                <Button type="submit" className="mt-5 block mx-auto">
                    Apply Changes
                </Button>
            </form>
        </Modal>
    );
}

export default TradeClosingModal;
