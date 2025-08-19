const DATA_CONTENT_VALUE_STRING = 'contentValue';
const CONTENT_WRAPPER_STRING = 'contentWrapper';
const CREATED_AT_STRING = 'createdAt';
const CHANGE_NOTE_STRING = 'changeNote';
const DATA_RESTRICTION_STRING = 'restrictions';

export const dataElementContentParser = (dataEle) => {
    return {
        get contentValue() {
            return dataEle?.[CONTENT_WRAPPER_STRING]?.[DATA_CONTENT_VALUE_STRING] ?? "";
        },
        get createdAt() {
            return dataEle?.[CONTENT_WRAPPER_STRING]?.[CREATED_AT_STRING] ?? "";
        },
        get changeNote() {
            return dataEle?.[CONTENT_WRAPPER_STRING]?.[CHANGE_NOTE_STRING] ?? "";
        },
        get textRestrictions() {
            return dataEle?.[DATA_RESTRICTION_STRING] ?? {};
        },
    };
};
