// material-ui
import { useTheme } from '@mui/material/styles';

/**
 * if you want to use image instead of <svg> uncomment following.
 *
 * import logoDark from 'assets/images/logo-dark.svg';
 * import logo from 'assets/images/logo.svg';
 *
 */

// ==============================|| LOGO SVG ||============================== //

const Logo = () => {
  const theme = useTheme();
  const fontWeight = '24px'
  return (
    <div style={{ display: 'flex', alignItems: 'center', margin: '5px', height: '40px' }}>
      <img
        src="/logo192.png"
        alt="Logo"
        style={{ width: fontWeight, height: fontWeight, borderRadius: '10px' }}
      />
      &nbsp;&nbsp;
      <h1 style={{ fontSize: '20px', fontFamily: 'Roboto, sans-serif', fontWeight: '600', marginRight: '5px' }}>
        CGV 도우미
      </h1>
    </div>
  );
};

export default Logo;
