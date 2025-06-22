import { Modal, NumberInput, Button } from '@mantine/core';
import { useForm } from '@mantine/form';
import { TbCheck } from "react-icons/tb";
import StyledActionButton from "@components/Common/StyledActionButton";

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
        <Modal 
            opened={opened} 
            onClose={onClose} 
            title="Closing Price" 
            centered
            styles={{
                title: {
                    fontSize: '18px',
                    fontWeight: '600',
                    color: '#374151',
                    textAlign: 'center',
                }
            }}
        >
            <form onSubmit={form.onSubmit(handleSubmit)}>
                <NumberInput
                    placeholder="Closing price..."
                    thousandSeparator=","
                    hideControls
                    {...form.getInputProps('closePrice')}
                    className="mb-1 max-w-xs block mx-auto"
                />
                <StyledActionButton 
                    type="submit" 
                    icon={<TbCheck size={20} />}
                    className="mt-5 block mx-auto"
                >
                    Submit
                </StyledActionButton>
            </form>
        </Modal>
    );
}

export default TradeClosingModal;
