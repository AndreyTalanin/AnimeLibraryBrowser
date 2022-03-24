import React, { useEffect, useState } from "react";
import { Table } from "react-bootstrap";
import Release from "../entities/Release";
import ReleaseDetailsRow from "./ReleaseDetailsRow";
import ReleaseHeaderRow from "./ReleaseHeaderRow";
import { getReleaseAsync } from "../requests/releaseRequests";

export interface ReleaseTableProps {
  releases: Release[];
  advancedModeEnabled: boolean;
}

const ReleaseTable = (props: ReleaseTableProps) => {
  const [tableData, setTableData] = useState<{ release: Release; expanded: boolean }[]>([]);

  useEffect(() => {
    if (tableData.length != props.releases.length) {
      setTableData(props.releases.map((release) => ({ release: release, expanded: false })));
    }
  }, [props.releases]);

  const expandReleaseDetailsRow = (release: Release) => {
    if (!release.fileGroups?.length) {
      getReleaseAsync(release.title)
        .then((release) => {
          setTableData(tableData.map((tableRow) => (tableRow.release.title == release.title ? { release: release, expanded: true } : tableRow)));
        })
        .catch((error) => alert(error));
    } else {
      setTableData(tableData.map((tableRow) => (tableRow.release.title == release.title ? { release: release, expanded: true } : tableRow)));
    }
  };
  const collapseReleaseDetailsRow = (release: Release) => {
    setTableData(tableData.map((tableRow) => (tableRow.release.title == release.title ? { release: release, expanded: false } : tableRow)));
  };

  return (
    <Table striped bordered size="sm">
      <thead>
        <tr>
          <th scope="col">Title</th>
          <th scope="col">Year</th>
          <th scope="col">Type</th>
          <th scope="col">Resolution</th>
          <th scope="col">Video Encoder</th>
          <th scope="col">Audio Encoder</th>
          <th scope="col">Actions</th>
        </tr>
      </thead>
      <tbody>
        {tableData.map(({ release, expanded }) => (
          <>
            <ReleaseHeaderRow release={release} expanded={expanded} onCollapsed={collapseReleaseDetailsRow} onExpanded={expandReleaseDetailsRow} />
            {expanded && <ReleaseDetailsRow release={release} advancedModeEnabled={props.advancedModeEnabled} />}
          </>
        ))}
      </tbody>
    </Table>
  );
};

export default ReleaseTable;
