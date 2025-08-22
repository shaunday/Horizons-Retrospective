import PropTypes from "prop-types";

// Single filter shape (used across all subcomponents)
export const filterShape = PropTypes.shape({
    id: PropTypes.string.isRequired,
    title: PropTypes.string,
    type: PropTypes.oneOf(["enum", "text", "dateRange"]),
    value: PropTypes.any,
    restrictions: PropTypes.arrayOf(PropTypes.string),
});

// Parent FilterSelector props
export const filterSelectorPropTypes = {
    filters: PropTypes.arrayOf(
        PropTypes.shape({
            field: PropTypes.string.isRequired,
            value: PropTypes.any,
        })
    ).isRequired,
    onAdd: PropTypes.func.isRequired,
    onRemove: PropTypes.func.isRequired,
    filterDefinitions: PropTypes.arrayOf(filterShape).isRequired,
};

// Subcomponent props
export const filterDateInputPropTypes = {
    filter: PropTypes.shape({
        id: PropTypes.string.isRequired,
        title: PropTypes.string.isRequired,
        value: PropTypes.any,
    }).isRequired,
};

export const filterSegmentedControlPropTypes = {
    filter: PropTypes.shape({
        id: PropTypes.string.isRequired,
        value: PropTypes.any,
    }).isRequired,
    data: PropTypes.arrayOf(
        PropTypes.shape({
            label: PropTypes.string.isRequired,
            value: PropTypes.string.isRequired,
        })
    ),
};

export const filterTextInputPropTypes = {
    filter: PropTypes.shape({
        id: PropTypes.string.isRequired,
        title: PropTypes.string.isRequired,
        value: PropTypes.string,
    }).isRequired,
};
