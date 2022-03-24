import React, { useEffect, useState } from "react";
import { Card, Container, Navbar } from "react-bootstrap";
import AdvancedModeSwitch from "./components/AdvancedModeSwitch";
import Release from "./entities/Release";
import ReleaseTable from "./components/ReleaseTable";
import { getAllReleasesAsync } from "./requests/releaseRequests";
import "./main.css";

const Application = (): JSX.Element => {
  const [releases, setReleases] = useState<Release[]>([]);
  const [releasesLoaded, setReleasesLoaded] = useState<boolean>(false);
  const [advancedModeEnabled, setAdvancedModeEnabled] = useState<boolean>(false);

  useEffect(() => {
    if (!releasesLoaded) {
      getAllReleasesAsync().then((releases) => {
        setReleases(releases);
        setReleasesLoaded(true);
      });
    }
  }, [releases]);

  return (
    <>
      <Navbar bg="light">
        <Container>
          <Navbar.Brand>Anime Library Browser</Navbar.Brand>
        </Container>
      </Navbar>
      <ReleaseTable releases={releases} advancedModeEnabled={advancedModeEnabled} />
      <AdvancedModeSwitch advancedModeEnabled={advancedModeEnabled} advancedModeToggled={(enabled) => setAdvancedModeEnabled(enabled)} />
      <Card body border="light" className="text-center">
        <a target="_blank" rel="noopener noreferrer" href="https://github.com/AndreyTalanin/AnimeLibraryBrowser">
          Anime Library Browser
        </a>
        &nbsp;-&nbsp;Copyright @ 2022 Andrey Talanin, Leonid Yakhtin, Maxim Zarkov. Built with ASP.NET Core, TypeScript &#38; React.
      </Card>
    </>
  );
};

export default Application;
