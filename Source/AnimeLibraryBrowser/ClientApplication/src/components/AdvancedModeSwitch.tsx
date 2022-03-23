import React from "react";
import { downloadDirectoryAsync, downloadFileAsync } from "../requests/contentRequests";

export interface AdvancedModeSwitchProps {
  advancedModeEnabled: boolean;
  advancedModeToggled: (enabled: boolean) => void;
}

const AdvancedModeSwitch = (props: AdvancedModeSwitchProps) => {
  const downloadCustomFile = () => {
    const relativePath = prompt("Enter the relative path:");
    if (relativePath) {
      downloadFileAsync(relativePath).catch((error) => alert(error));
    }
  };
  const downloadCustomDirectory = () => {
    const relativePath = prompt("Enter the relative path:");
    if (relativePath) {
      downloadDirectoryAsync(relativePath).catch((error) => alert(error));
    }
  };

  return (
    <>
      <div>
        <input type="checkbox" name="enable-advanced-mode-checkbox" onChange={(e) => props.advancedModeToggled(e.target.checked)} />
        <label htmlFor="enable-advanced-mode-checkbox">Enable Advanced Mode</label>
      </div>
      {props.advancedModeEnabled && (
        <>
          <button onClick={() => downloadCustomFile()}>Download Custom File</button>
          <button onClick={() => downloadCustomDirectory()}>Download Custom Directory</button>
        </>
      )}
    </>
  );
};

export default AdvancedModeSwitch;
