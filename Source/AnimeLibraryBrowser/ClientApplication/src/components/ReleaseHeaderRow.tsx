import React from "react";
import Release from "../entities/Release";

export interface ReleaseHeaderRowProps {
  release: Release;
  expanded: boolean;
  onExpanded: (release: Release) => void;
  onCollapsed: (release: Release) => void;
}

const ReleaseHeaderRow = (props: ReleaseHeaderRowProps) => {
  const { release, expanded, onExpanded, onCollapsed } = props;

  return (
    <tr>
      <td>{release.title}</td>
      <td>{release.year}</td>
      <td>{release.type}</td>
      <td>{`${release.frameWidth}x${release.frameHeight}`}</td>
      <td>{release.videoEncoder}</td>
      <td>{release.audioEncoder}</td>
      <td>
        <button onClick={() => (expanded ? onCollapsed(release) : onExpanded(release))}>{expanded ? "Collapse" : "Expand"}</button>
      </td>
    </tr>
  );
};

export default ReleaseHeaderRow;
