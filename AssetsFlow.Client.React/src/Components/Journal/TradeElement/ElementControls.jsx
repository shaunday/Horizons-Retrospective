import React from "react";
import SuccessMessage from "@components/SuccessMessage";
import { useElementActivationMutation } from "@hooks/Element/useElementActivationMutation";
import { canActivateElement } from "@services/elementActivation";
import {useCacheElementActivation} from "@hooks/Element/useCacheElementActivation"

const MemoizedSuccessMessage = React.memo(SuccessMessage);

function ElementControls({ tradeElement }) {
  const allowActivate = canActivateElement(tradeElement);
  const { setNewActiveState } = useCacheElementActivation(tradeElement);

  const { elementActivationMutation, processing, success } =
    useElementActivationMutation(() => {
      setNewActiveState(true);
    });

  const initiateActivation = () => {
    elementActivationMutation.mutate(tradeElement);
  };

  return (
    <>
      <div style={{ textAlign: "right", marginRight: "10px" }}>
        {allowActivate && <button
          className="button-38"
          type="button"
          style={{ display: "inline-block", marginRight: "10px" }}
          onClick={initiateActivation}
        >
          Activate
        </button>}
      </div>
      {processing && <div className="spinner">Processing...</div>}
      {success && <MemoizedSuccessMessage />}
    </>
  );
}

export default React.memo(ElementControls);
