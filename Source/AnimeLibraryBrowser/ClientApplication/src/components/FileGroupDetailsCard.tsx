import React from "react";
import { Button, Card, Stack, Table } from "react-bootstrap";
import File from "../entities/File";
import FileGroup from "../entities/FileGroup";
import FileType from "../entities/FileType";
import { downloadDirectoryAsync, downloadFileAsync } from "../requests/contentRequests";
import "./FileGroupDetailsCard.css";

export interface FileGroupDetailsCardProps {
  fileGroup: FileGroup;
  advancedModeEnabled: boolean;
}

const FileGroupDetailsCard = (props: FileGroupDetailsCardProps): JSX.Element => {
  const downloadFile = (file: File) => {
    downloadFileAsync(file.relativePath).catch((error) => alert(error));
  };
  const downloadFileGroup = (fileGroup: FileGroup) => {
    downloadDirectoryAsync(fileGroup.relativePath).catch((error) => alert(error));
  };
  const copyFtpLinkToClipboard = (ftpLink: string) => {
    navigator.clipboard.writeText(ftpLink).catch((error) => alert(error));
  };

  return (
    <Card border="secondary">
      <Card.Body>
        <Card.Title>{props.fileGroup.name}</Card.Title>
        {props.advancedModeEnabled && <Card.Subtitle>{`Relative Directory Path: "${props.fileGroup.relativePath}"`}</Card.Subtitle>}
        <Table striped bordered hover size="sm" style={{ marginTop: "16px" }}>
          <thead>
            <tr>
              <th scope="col">File Name</th>
              <th scope="col">Type</th>
              {props.advancedModeEnabled && <th scope="col">Length</th>}
              <th scope="col">Actions</th>
            </tr>
          </thead>
          <tbody>
            {props.fileGroup.files.map((file) => (
              <tr>
                <td>{file.name}</td>
                <td className="td-file-type">{FileType[file.type]}</td>
                {props.advancedModeEnabled && <td className="td-file-length">{file.length}</td>}
                <td className="td-file-actions">
                  <Stack direction="horizontal" gap={1}>
                    <Button variant="outline-primary" size="sm" onClick={() => downloadFile(file)}>
                      Download File
                    </Button>
                    <Button variant="outline-primary" size="sm" onClick={() => copyFtpLinkToClipboard(file.ftpLink)}>
                      Copy FTP Link
                    </Button>
                  </Stack>
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
        <Button variant="primary" size="sm" onClick={() => downloadFileGroup(props.fileGroup)}>
          Download All Files
        </Button>
      </Card.Body>
    </Card>
  );
};

export default FileGroupDetailsCard;
