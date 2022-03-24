import React from "react";
import FileGroupDetailsCard from "./FileGroupDetailsCard";
import Release from "../entities/Release";
import { Stack } from "react-bootstrap";

export interface ReleaseDetailsRowProps {
  release: Release;
  advancedModeEnabled: boolean;
}

const ReleaseDetailsRow = (props: ReleaseDetailsRowProps): JSX.Element => {
  return (
    <tr>
      <td colSpan={Number.MAX_SAFE_INTEGER}>
        <Stack gap={2}>
          {props.release.fileGroups?.map((fileGroup) => (
            <FileGroupDetailsCard fileGroup={fileGroup} advancedModeEnabled={props.advancedModeEnabled} />
          ))}
        </Stack>
      </td>
    </tr>
  );
};

export default ReleaseDetailsRow;
