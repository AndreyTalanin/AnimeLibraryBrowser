import React from "react";
import FileGroupDetailsCard from "./FileGroupDetailsCard";
import Release from "../entities/Release";

export interface ReleaseDetailsRowProps {
  release: Release;
  advancedModeEnabled: boolean;
}

const ReleaseDetailsRow = (props: ReleaseDetailsRowProps): JSX.Element => {
  return (
    <>
      <tr>
        <td colSpan={Number.MAX_SAFE_INTEGER}>
          {props.release.fileGroups?.map((fileGroup) => (
            <FileGroupDetailsCard fileGroup={fileGroup} advancedModeEnabled={props.advancedModeEnabled} />
          ))}
        </td>
      </tr>
    </>
  );
};

export default ReleaseDetailsRow;
