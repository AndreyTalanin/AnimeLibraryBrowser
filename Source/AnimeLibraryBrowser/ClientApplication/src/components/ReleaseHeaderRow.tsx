import React from "react";
import { Button } from "react-bootstrap";
import Release from "../entities/Release";
import "./ReleaseHeaderRow.css";

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
      <td className="td-release-actions">
        <Button variant="outline-primary" size="sm" className="button-expand" onClick={() => (expanded ? onCollapsed(release) : onExpanded(release))}>
          {expanded ? "Collapse" : "Expand"}
        </Button>
      </td>
    </tr>
  );
};

export default ReleaseHeaderRow;
