import React from "react";
import File from "../entities/File";
import FileGroup from "../entities/FileGroup";
import FileType from "../entities/FileType";
import { downloadDirectoryAsync, downloadFileAsync } from "../requests/contentRequests";

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
    <>
      <div>
        {props.fileGroup.name}
        <button onClick={() => downloadFileGroup(props.fileGroup)}>Download All Files</button>
      </div>
      {props.advancedModeEnabled && <div>{`Relative Path: "${props.fileGroup.relativePath}"`}</div>}
      <table>
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
            <>
              <tr>
                <td>{file.name}</td>
                <td>{FileType[file.type]}</td>
                {props.advancedModeEnabled && <td>{file.length}</td>}
                <td>
                  <button onClick={() => downloadFile(file)}>Download File</button>
                  <button onClick={() => copyFtpLinkToClipboard(file.ftpLink)}>Copy FTP Link</button>
                </td>
              </tr>
            </>
          ))}
        </tbody>
      </table>
    </>
  );
};

export default FileGroupDetailsCard;
