import { Modal, NumberInput } from "@mantine/core";
import { useForm } from "@mantine/form";
import { TbCheck } from "react-icons/tb";
import PropTypes from "prop-types";
import StyledActionButton from "@components/Common/StyledActionButton";

function TradeClosingModal({ opened, onClose, onSubmit }) {
  const form = useForm({
    initialValues: {
      closePrice: "",
    },
    validate: {
      closePrice: (val) => {
        if (val === null || val === undefined || String(val).trim() === "") {
          return "Closing Price cannot be empty";
        }
        if (isNaN(val) || !isFinite(val)) {
          return "Closing Price must be a valid number";
        }
        if (Number(val) === 0) {
          return "Closing Price cannot be zero";
        }
        if (Number(val) <= 0) {
          return "Closing Price must be greater than zero";
        }
        return null;
      },
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
      centered
      title={<div className="text-lg font-semibold text-gray-700 text-center">Closing Price</div>}
    >
      <form onSubmit={form.onSubmit(handleSubmit)} className="mt-4 space-y-4">
        <NumberInput
          placeholder="Closing price..."
          thousandSeparator=","
          hideControls
          {...form.getInputProps("closePrice")}
          className="max-w-xs mx-auto"
        />

        <div className="flex justify-center">
          <StyledActionButton type="submit" icon={<TbCheck size={20} />}>
            Submit
          </StyledActionButton>
        </div>
      </form>
    </Modal>
  );
}
TradeClosingModal.propTypes = {
  opened: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  onSubmit: PropTypes.func.isRequired,
};

export default TradeClosingModal;
