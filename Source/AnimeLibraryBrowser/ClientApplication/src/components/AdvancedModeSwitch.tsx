import React from "react";
import { Accordion, Button, Form, Stack } from "react-bootstrap";
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
    <Accordion style={{ marginBottom: "16px" }}>
      <Accordion.Item eventKey="0">
        <Accordion.Header>Advanced Mode</Accordion.Header>
        <Accordion.Body>
          <Stack gap={1}>
            <Form.Check
              type="checkbox"
              label="Enable Advanced Mode (Allows you to see file lengths, relative paths, as well as to download custom files and directories)"
              onChange={(e) => props.advancedModeToggled(e.target.checked)}
            />
            {props.advancedModeEnabled && (
              <Stack direction="horizontal" gap={1}>
                <Button variant="outline-primary" size="sm" onClick={() => downloadCustomFile()}>
                  Download Custom File
                </Button>
                <Button variant="outline-primary" size="sm" onClick={() => downloadCustomDirectory()}>
                  Download Custom Directory
                </Button>
              </Stack>
            )}
          </Stack>
        </Accordion.Body>
      </Accordion.Item>
    </Accordion>
  );
};

export default AdvancedModeSwitch;
