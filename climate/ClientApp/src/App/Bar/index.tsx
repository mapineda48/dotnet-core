import * as React from "react";
import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import MenuItem from "@mui/material/MenuItem";
import Menu from "@mui/material/Menu";
import Drawer from "@mui/material/Drawer";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemText from "@mui/material/ListItemText";
import Divider from "@mui/material/Divider";
import HistoryIcon from "@mui/icons-material/Restore";
import SearchIcon from "@mui/icons-material/Search";
import MoreIcon from "@mui/icons-material/MoreVert";
import { Search, StyledInputBase, SearchIconWrapper } from "./style";

export default function PrimarySearchAppBar(props: Props) {
  const [location, setLocation] = React.useState("");

  const [isOpenHistory, setOpenHisotry] = React.useState(false);

  const [open, hidden] = React.useMemo(() => {
    return [() => setOpenHisotry(true), () => setOpenHisotry(false)];
  }, [setOpenHisotry]);

  const openHistory = props.disabled ? undefined : open;

  const [mobileMoreAnchorEl, setMobileMoreAnchorEl] =
    React.useState<null | HTMLElement>(null);

  const isMobileMenuOpen = Boolean(mobileMoreAnchorEl);

  const handleMobileMenuClose = () => {
    setMobileMoreAnchorEl(null);
  };

  const handleMobileMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setMobileMoreAnchorEl(event.currentTarget);
  };

  const mobileMenuId = "primary-search-account-menu-mobile";
  const renderMobileMenu = (
    <Menu
      anchorEl={mobileMoreAnchorEl}
      anchorOrigin={{
        vertical: "top",
        horizontal: "right",
      }}
      id={mobileMenuId}
      keepMounted
      transformOrigin={{
        vertical: "top",
        horizontal: "right",
      }}
      open={isMobileMenuOpen}
      onClose={handleMobileMenuClose}
    >
      <MenuItem
        onClick={() => {
          open();
          handleMobileMenuClose();
        }}
      >
        <IconButton
          size="large"
          edge="end"
          onClick={openHistory}
          color="inherit"
        >
          <HistoryIcon />
        </IconButton>
        <p>history</p>
      </MenuItem>
    </Menu>
  );

  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="fixed">
        <Toolbar>
          <Typography
            variant="h6"
            noWrap
            component="div"
            sx={{ display: { xs: "none", sm: "block" } }}
          >
            Climate
          </Typography>
          <Search
            onSubmit={(e) => {
              e.preventDefault();
              if (!location) return;
              setLocation("");
              props.onSearch(location.toLocaleLowerCase());
            }}
          >
            <SearchIconWrapper>
              <SearchIcon />
            </SearchIconWrapper>
            <StyledInputBase
              placeholder="Location..."
              inputProps={{ "aria-label": "search" }}
              value={location}
              onChange={({ currentTarget: { value } }) => setLocation(value)}
            />
          </Search>
          <Box sx={{ flexGrow: 1 }} />
          <Box sx={{ display: { xs: "none", md: "flex" } }}>
            <IconButton
              size="large"
              edge="end"
              onClick={openHistory}
              color="inherit"
            >
              <HistoryIcon />
            </IconButton>
          </Box>
          <Box sx={{ display: { xs: "flex", md: "none" } }}>
            <IconButton
              size="large"
              aria-label="show more"
              onClick={props.disabled ? undefined : handleMobileMenuOpen}
              color="inherit"
            >
              <MoreIcon />
            </IconButton>
          </Box>
        </Toolbar>
      </AppBar>
      {renderMobileMenu}
      <Drawer anchor="right" open={isOpenHistory} onClose={hidden}>
        <List>
          {(!props.historys.length && (
            <ListItem>
              <ListItemText primary="Sin Historial" />
            </ListItem>
          )) || (
            <ListItem>
              <ListItemText primary="Historial" />
            </ListItem>
          )}
          <Divider />
          {props.historys.map((location, index) => (
            <ListItem button key={index}>
              <ListItemText
                primary={location}
                onClick={() => {
                  props.onSearch(location);
                  hidden();
                }}
              />
            </ListItem>
          ))}
        </List>
      </Drawer>
    </Box>
  );
}

/**
 * Types
 */
export interface Props {
  onSearch: (location: string) => void;
  historys: string[];
  disabled: boolean;
}
